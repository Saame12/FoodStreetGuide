public class Place
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; } // dùng tạm từ API

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string AudioUrl { get; set; }

    public string Category { get; set; }

    public double Radius { get; set; } = 0.1;
}