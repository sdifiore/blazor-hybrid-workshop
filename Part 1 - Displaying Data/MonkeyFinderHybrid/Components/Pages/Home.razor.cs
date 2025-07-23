using Microsoft.FluentUI.AspNetCore.Components;

using MonkeyFinderHybrid.Components.Controls;

namespace MonkeyFinderHybrid.Components.Pages
{
    public partial class Home
    {
        private List<Monkey> _monkeys = [];
        private Monkey DialogData { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            // Load monkeys from MonkeyService
            _monkeys = await monkeyService.GetMonkeys();
        }

        private async Task AddMonkey()
        {
            // MAUI Debug console
            Debug.WriteLine("Add Monkey");


            // Create a new instance of DialogData to allow the user to cancel the update
            var data = new Monkey();
            IDialogReference dialog = await DialogService.ShowDialogAsync<SimpleCustomizedDialog>(data, new DialogParameters
            {
                Title = "Add New Monkey",
                PreventDismissOnOverlayClick = true,
                PreventScroll = true
            });

            DialogResult result = await dialog.Result;

            if (!result.Cancelled && result.Data is not null)
            {
                DialogData = (Monkey)result.Data;
                _ = monkeyService.AddMonkey(DialogData);
                _monkeys = await monkeyService.GetMonkeys();
            }
        }

        private void GoToDetails(Monkey monkey)
        {
            // Navigate to the details page for the selected monkey
            NavManager.NavigateTo($"details/{monkey.Name}");
        }
        private async Task FindMonkey()
        {
            try
            {
                //Get cached location, else get current location
                Location? location = await geolocation.GetLastKnownLocationAsync();

                if (location is null)
                {
                    location = await geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                // Find the closest monkey
                Monkey? closestMonkey = _monkeys.OrderBy(m => location.CalculateDistance(
                new Location(m.Latitude, m.Longitude), DistanceUnits.Kilometers))
                    .FirstOrDefault();

                string closestMonkeyMessage = string.Empty;

                if (closestMonkey is not null)
                {
                    closestMonkeyMessage = $"Closest monkey is {closestMonkey.Name} at {closestMonkey.Location}.";
                }

                else
                {
                    closestMonkeyMessage = "No monkeys found.";
                }

                await ((Application)app).Windows[0].Page!.DisplayAlert("Closest Monkey",
                       closestMonkeyMessage, "Ok");
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to query location: {ex.Message}");
                await ((Application)app).Windows[0].Page!.DisplayAlert("Error!",
                       ex.Message, "Ok");
            }
        }
    }
}