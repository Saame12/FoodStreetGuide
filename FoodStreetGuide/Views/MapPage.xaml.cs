using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Media;
using System.Threading;

namespace FoodStreetGuide.Views;

public partial class MapPage : ContentPage
{
    List<Place> places = new();
    Place selectedPlace;
    CancellationTokenSource cts;
    public MapPage()
	{
		InitializeComponent();
        // DATA
        places.Add(new Place
        {
            Name = "Dinh Độc Lập",
            DescriptionVI = "Di tích lịch sử nổi tiếng tại TP.HCM",
            Latitude = 10.7769,
            Longitude = 106.6953
        });

        places.Add(new Place
        {
            Name = "Chợ Bến Thành",
            DescriptionVI = "Khu chợ nổi tiếng Sài Gòn",
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
    }
        
    // 📄 XEM TEXT
    private void OnShowText(object sender, EventArgs e)
    {
        // chỉ hiển thị text, không làm gì thêm
    }
   
    // 📍 GPS
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
            ShowPanel(selectedPlace);
        }

        e.HideInfoWindow = true;
    }

    // 🎬 SHOW PANEL
    private async Task ShowPanel(Place place)
    {
        panelName.Text = place.Name;
        panelAddress.Text = "TP.HCM";
        panelDescription.Text = place.DescriptionVI;

        await infoPanel.TranslateTo(0, 0, 300);

        StartAutoSpeak(place);
    }

    // ⏱️ AUTO SPEAK
    private async void StartAutoSpeak(Place place)
    {
        cts?.Cancel();
        cts = new CancellationTokenSource();

        try
        {
            await Task.Delay(90000, cts.Token);
            await TextToSpeech.Default.SpeakAsync(place.DescriptionVI);
        }
        catch { }
    }

    // 🔊 NGHE NGAY
    private async void OnSpeakNow(object sender, EventArgs e)
    {
        cts?.Cancel();

        if (selectedPlace != null)
        {
            await TextToSpeech.Default.SpeakAsync(selectedPlace.DescriptionVI);
        }
    }

    // ❌ HỦY
    private async void OnCancel(object sender, EventArgs e)
    {
        cts?.Cancel();
        await infoPanel.TranslateTo(0, 300, 300);
    }

}