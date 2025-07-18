// Flight.cs
public class Flight
{
	public string FlightCode { get; set; }
	public string Airline { get; set; }
	public string Origin { get; set; }
	public string Destination { get; set; }
	public string Day { get; set; }
	public string Time { get; set; }
	public int TotalSeats { get; set; }
	public int AvailableSeats { get; set; }
	public decimal Cost { get; set; }

	public Flight(string flightCode, string airline, string origin, string destination,
				 string day, string time, int totalSeats, decimal cost)
	{
		FlightCode = flightCode;
		Airline = airline;
		Origin = origin;
		Destination = destination;
		Day = day;
		Time = time;
		TotalSeats = totalSeats;
		AvailableSeats = totalSeats;
		Cost = cost;
	}

	public override string ToString()
	{
		return $"{FlightCode} - {Airline} - {Origin} to {Destination} on {Day} at {Time} - ${Cost}";
	}
}