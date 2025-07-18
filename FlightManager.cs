// FlightManager.cs
public class FlightManager
{
    private List<Flight> flights;
    private List<Airport> airports;

    public FlightManager()
    {
        flights = new List<Flight>();
        airports = new List<Airport>();
        LoadAirports();
        LoadFlights();
    }

    private void LoadAirports()
    {
        // Load from airports.csv
        var lines = File.ReadAllLines("airports.csv");
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            airports.Add(new Airport(parts[0], parts[1]));
        }
    }

    private void LoadFlights()
    {
        // Load from flights.csv
        var lines = File.ReadAllLines("flights.csv");
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            flights.Add(new Flight(
                parts[0], parts[1], parts[2], parts[3], parts[4], parts[5],
                int.Parse(parts[6]), decimal.Parse(parts[7])));
        }
    }

    public List<Flight> FindFlights(string origin, string destination, string day)
    {
        return flights.Where(f =>
            f.Origin.Equals(origin, StringComparison.OrdinalIgnoreCase) &&
            f.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase) &&
            f.Day.Equals(day, StringComparison.OrdinalIgnoreCase) &&
            f.AvailableSeats > 0).ToList();
    }

    public List<Airport> GetAirports()
    {
        return airports;
    }
}