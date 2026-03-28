public class Place
{
    public string Name { get; set; }
    public string DescriptionVI { get; set; }
    public string DescriptionEN { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public double Radius { get; set; } = 5; // KLmét
}