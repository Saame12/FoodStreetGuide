using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Media;
using System.Threading;
using System.Net.Http.Json;

namespace FoodStreetGuide.Views;

public partial class MapPage : ContentPage
{
    List<Place> places = new();
    Place selectedPlace;
    HttpClient client = new HttpClient();
    CancellationTokenSource cts;
    bool isTracking = false;

    // 🔥 chống lặp
    HashSet<string> visited = new();
    Dictionary<string, DateTime> pending = new();
    public MapPage()
	{
		InitializeComponent();
        // DATA
        places.Add(new Place
        {
            Name = "Dinh Độc Lập",
            Description = "Di tích lịch sử nổi tiếng tại TP.HCM",
            Latitude = 10.7769,
            Longitude = 106.6953
        });

        places.Add(new Place
        {
            Name = "Chợ Bến Thành",
            Description = "Khu chợ nổi tiếng Sài Gòn",
            Latitude = 10.772,
            Longitude = 106.698
        });
        // ADD PIN
        foreach (var p in places)
        {
            var pin = new Pin
            {
                Label = "❗ " + p.Name,
                Location = new Location(p.Latitude, p.Longitude)
            };

            pin.MarkerClicked += OnPinClicked;
            map.Pins.Add(pin);
        }
        LoadPlacesNearMe();
    }

    // 📍 START GPS
    private async void OnStartTracking(object sender, EventArgs e)
    {
        var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert("Lỗi", "Chưa cấp quyền GPS", "OK");
            return;
        }

        var location = await Geolocation.GetLocationAsync();

        if (location != null)
        {
            map.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Location(location.Latitude, location.Longitude),
                Distance.FromMeters(500)));
        }

        StartRealtimeTracking();
    }

    // 📍 CLICK PIN
    private void OnPinClicked(object sender, PinClickedEventArgs e)
    {
        var pin = sender as Pin;
        if (pin == null) return;

        var name = pin.Label.Replace("❗ ", "");
        selectedPlace = places.FirstOrDefault(p => p.Name == name);

        if (selectedPlace != null)
        {
            _ = ShowPanel(selectedPlace);
        }

        e.HideInfoWindow = true;
    }

    // 🎬 SHOW PANEL
    private async Task ShowPanel(Place place)
    {
        cts?.Cancel(); // 🔥 tránh chồng audio

        panelName.Text = place.Name;
        panelAddress.Text = "TP.HCM";
        panelDescription.Text = place.Description;

        await infoPanel.TranslateTo(0, 0, 300);

        StartAutoSpeak(place);
    }

    // ❌ HIDE PANEL
    private async Task HidePanel()
    {
        await infoPanel.TranslateTo(0, 300, 300);
    }

    // 📏 DISTANCE (METERS)
    double GetDistance(Location a, Location b)
    {
        return Location.CalculateDistance(a, b, DistanceUnits.Kilometers);
    }

    // 🚶 REALTIME GPS
    async void StartRealtimeTracking()
    {
        if (isTracking) return;

        isTracking = true;

        while (isTracking)
        {
            try
            {
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    var userLoc = new Location(location.Latitude, location.Longitude);

                    foreach (var place in places)
                    {
                        var placeLoc = new Location(place.Latitude, place.Longitude);
                        double distance = GetDistance(userLoc, placeLoc);

                        // 👉 VÀO VÙNG
                        if (distance <= 0.005)
                        {
                            if (!pending.ContainsKey(place.Name))
                            {
                                pending[place.Name] = DateTime.Now;
                                DebounceEnter(place);
                            }
                        }
                        else
                        {
                            // 👉 RA KHỎI VÙNG
                            pending.Remove(place.Name);

                            if (selectedPlace == place)
                            {
                                selectedPlace = null;
                                await HidePanel();
                            }
                        }
                    }
                }
            }
            catch
            {
                // tránh crash GPS
            }
            await LoadPlacesNearMe();
            await Task.Delay(5000);
        }
    }

    // ⏱️ debounce 3s
    async void DebounceEnter(Place place)
    {
        await Task.Delay(3000);

        if (pending.ContainsKey(place.Name))
        {
            if (!visited.Contains(place.Name))
            {
                visited.Add(place.Name);
                selectedPlace = place;

                await ShowPanel(place);
            }
        }
    }

    // 🔊 AUTO SPEAK
    private async void StartAutoSpeak(Place place)
    {
        cts?.Cancel();
        cts = new CancellationTokenSource();

        try
        {
            await Task.Delay(90000, cts.Token);
            await TextToSpeech.Default.SpeakAsync(place.Description);
        }
        catch { }
    }
    async Task LoadPlacesNearMe()
    {
        try
        {
            var location = await Geolocation.GetLocationAsync();

            if (location == null) return;

            double lat = location.Latitude;
            double lng = location.Longitude;

            var data = await client.GetFromJsonAsync<List<Place>>(
                $"http://10.0.2.2:5550/api/v1/poi/near?lat={lat}&lng={lng}"
            );

            if (data != null)
            {
                places = data;

                map.Pins.Clear();

                foreach (var p in places)
                {
                    var pin = new Pin
                    {
                        Label = "📍 " + p.Name,
                        Location = new Location(p.Latitude, p.Longitude)
                    };

                    pin.MarkerClicked += OnPinClicked;
                    map.Pins.Add(pin);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi API", ex.Message, "OK");
        }
    }
    // 🔊 NGHE NGAY
    private async void OnSpeakNow(object sender, EventArgs e)
    {
        cts?.Cancel();

        if (selectedPlace != null)
        {
            await TextToSpeech.Default.SpeakAsync(selectedPlace.Description);
        }
    }
    private void OnPanelDragged(object sender, PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Running)
        {
            infoPanel.TranslationY += e.TotalY;
        }
    }
    // 📄 XEM TEXT

    private void OnShowText(object sender, EventArgs e)
    {
        // có thể mở popup chi tiết sau
    }

    // ❌ HỦY
    private async void OnCancel(object sender, EventArgs e)
    {
        cts?.Cancel();
        selectedPlace = null;
        await HidePanel();
    }

}