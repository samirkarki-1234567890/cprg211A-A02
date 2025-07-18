// MainForm.cs
public partial class MainForm : Form
{
    private FlightManager flightManager;
    private ReservationManager reservationManager;
    private List<Flight> currentFlights;
    private List<Reservation> currentReservations;

    public MainForm()
    {
        InitializeComponent();
        flightManager = new FlightManager();
        reservationManager = new ReservationManager();

        // Setup UI
        SetupFindFlightsTab();
        SetupFindReservationsTab();
    }

    private void SetupFindFlightsTab()
    {
        // Populate airport dropdowns
        var airports = flightManager.GetAirports();
        cmbOrigin.DataSource = airports;
        cmbDestination.DataSource = airports.ToList(); // Create a copy

        // Populate days dropdown
        cmbDay.DataSource = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
    }

    private void SetupFindReservationsTab()
    {
        // Initialize reservation search controls
        // (Implementation depends on your specific UI design)
    }

    private void btnFindFlights_Click(object sender, EventArgs e)
    {
        string origin = (cmbOrigin.SelectedItem as Airport)?.Code;
        string destination = (cmbDestination.SelectedItem as Airport)?.Code;
        string day = cmbDay.SelectedItem as string;

        if (origin == destination)
        {
            MessageBox.Show("Origin and destination cannot be the same.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        currentFlights = flightManager.FindFlights(origin, destination, day);
        lstFlights.DataSource = currentFlights;
        lstFlights.DisplayMember = "ToString";
    }

    private void lstFlights_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedFlight = lstFlights.SelectedItem as Flight;
        if (selectedFlight != null)
        {
            txtFlightCode.Text = selectedFlight.FlightCode;
            txtAirline.Text = selectedFlight.Airline;
            txtDay.Text = selectedFlight.Day;
            txtTime.Text = selectedFlight.Time;
            txtCost.Text = selectedFlight.Cost.ToString("C");
        }
    }

    private void btnMakeReservation_Click(object sender, EventArgs e)
    {
        var selectedFlight = lstFlights.SelectedItem as Flight;
        if (selectedFlight == null)
        {
            MessageBox.Show("Please select a flight first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        string name = txtTravelerName.Text.Trim();
        string citizenship = txtCitizenship.Text.Trim();

        if (string.IsNullOrEmpty(name))
        {
            MessageBox.Show("Please enter traveler name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (string.IsNullOrEmpty(citizenship))
        {
            MessageBox.Show("Please enter citizenship.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            var reservation = reservationManager.MakeReservation(selectedFlight, name, citizenship);
            MessageBox.Show($"Reservation created successfully!\nReservation Code: {reservation.ReservationCode}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Clear fields
            txtTravelerName.Clear();
            txtCitizenship.Clear();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating reservation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnFindReservations_Click(object sender, EventArgs e)
    {
        string code = txtReservationCode.Text.Trim();
        string airline = txtReservationAirline.Text.Trim();
        string name = txtReservationName.Text.Trim();

        currentReservations = reservationManager.FindReservations(
            string.IsNullOrEmpty(code) ? null : code,
            string.IsNullOrEmpty(airline) ? null : airline,
            string.IsNullOrEmpty(name) ? null : name);

        lstReservations.DataSource = currentReservations;
        lstReservations.DisplayMember = "ToString";
    }

    private void lstReservations_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedReservation = lstReservations.SelectedItem as Reservation;
        if (selectedReservation != null)
        {
            txtReservationCodeDisplay.Text = selectedReservation.ReservationCode;
            txtReservationFlightCode.Text = selectedReservation.Flight.FlightCode;
            txtReservationAirlineDisplay.Text = selectedReservation.Flight.Airline;
            txtReservationCost.Text = selectedReservation.Flight.Cost.ToString("C");
            txtReservationName.Text = selectedReservation.TravelerName;
            txtReservationCitizenship.Text = selectedReservation.Citizenship;
            chkReservationActive.Checked = selectedReservation.IsActive;
        }
    }

    private void btnUpdateReservation_Click(object sender, EventArgs e)
    {
        var selectedReservation = lstReservations.SelectedItem as Reservation;
        if (selectedReservation == null)
        {
            MessageBox.Show("Please select a reservation first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        string name = txtReservationName.Text.Trim();
        string citizenship = txtReservationCitizenship.Text.Trim();
        bool isActive = chkReservationActive.Checked;

        if (string.IsNullOrEmpty(name))
        {
            MessageBox.Show("Please enter traveler name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (string.IsNullOrEmpty(citizenship))
        {
            MessageBox.Show("Please enter citizenship.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            reservationManager.UpdateReservation(selectedReservation, name, citizenship, isActive);
            MessageBox.Show("Reservation updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Refresh the list
            btnFindReservations_Click(null, null);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating reservation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}