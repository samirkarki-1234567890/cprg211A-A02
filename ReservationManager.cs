// ReservationManager.cs
public class ReservationManager
{
    private List<Reservation> reservations;
    private const string ReservationFile = "reservations.dat";

    public ReservationManager()
    {
        reservations = new List<Reservation>();
        LoadReservations();
    }

    private void LoadReservations()
    {
        if (File.Exists(ReservationFile))
        {
            try
            {
                using (var stream = File.Open(ReservationFile, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    reservations = (List<Reservation>)formatter.Deserialize(stream);
                }
            }
            catch
            {
                // If loading fails, start with empty list
                reservations = new List<Reservation>();
            }
        }
    }

    private void SaveReservations()
    {
        using (var stream = File.Open(ReservationFile, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, reservations);
        }
    }

    public string GenerateReservationCode()
    {
        // Format: LDDDD
        Random random = new Random();
        char letter = (char)('A' + random.Next(0, 26));
        string numbers = random.Next(0, 10000).ToString("D4");
        return $"{letter}{numbers}";
    }

    public Reservation MakeReservation(Flight flight, string travelerName, string citizenship)
    {
        if (flight == null)
            throw new ArgumentNullException("Flight cannot be null");
        if (string.IsNullOrEmpty(travelerName))
            throw new ArgumentException("Traveler name cannot be empty");
        if (string.IsNullOrEmpty(citizenship))
            throw new ArgumentException("Citizenship cannot be empty");
        if (flight.AvailableSeats <= 0)
            throw new Exception("Flight is fully booked");

        string code = GenerateReservationCode();
        var reservation = new Reservation(code, flight, travelerName, citizenship);
        flight.AvailableSeats--;
        reservations.Add(reservation);
        SaveReservations();
        return reservation;
    }

    public List<Reservation> FindReservations(string reservationCode = null, string airline = null, string travelerName = null)
    {
        return reservations.Where(r =>
            (string.IsNullOrEmpty(reservationCode) || r.ReservationCode.Equals(reservationCode, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(airline) || r.Flight.Airline.Equals(airline, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(travelerName) || r.TravelerName.Equals(travelerName, StringComparison.OrdinalIgnoreCase))
        ).ToList();
    }

    public void UpdateReservation(Reservation reservation, string newName, string newCitizenship, bool isActive)
    {
        if (reservation == null)
            throw new ArgumentNullException("Reservation cannot be null");
        if (string.IsNullOrEmpty(newName))
            throw new ArgumentException("Traveler name cannot be empty");
        if (string.IsNullOrEmpty(newCitizenship))
            throw new ArgumentException("Citizenship cannot be empty");

        reservation.TravelerName = newName;
        reservation.Citizenship = newCitizenship;

        if (reservation.IsActive && !isActive)
        {
            // If changing from active to inactive, free up a seat
            reservation.Flight.AvailableSeats++;
        }
        else if (!reservation.IsActive && isActive)
        {
            // If changing from inactive to active, take a seat
            if (reservation.Flight.AvailableSeats <= 0)
                throw new Exception("Flight is fully booked");
            reservation.Flight.AvailableSeats--;
        }

        reservation.IsActive = isActive;
        SaveReservations();
    }
}