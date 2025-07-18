// Reservation.cs
public class Reservation
{
    public string ReservationCode { get; set; }
    public Flight Flight { get; set; }
    public string TravelerName { get; set; }
    public string Citizenship { get; set; }
    public bool IsActive { get; set; }

    public Reservation(string reservationCode, Flight flight, string travelerName, string citizenship)
    {
        ReservationCode = reservationCode;
        Flight = flight;
        TravelerName = travelerName;
        Citizenship = citizenship;
        IsActive = true;
    }

    public override string ToString()
    {
        return $"{ReservationCode} - {TravelerName} - {Flight.FlightCode} - {(IsActive ? "Active" : "Inactive")}";
    }
}