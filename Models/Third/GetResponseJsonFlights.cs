namespace Models.Third
{
    /// <summary>
    /// This class is create only when create obj of API call (appsettings.json["AppSettings"]["urlAPIFlights"])
    /// </summary>
    public class GetJsonFlightResponse
    {
        public string DepartureStation { get; set; }
        public string ArrivalStation { get; set; }
        public string FlightCarrier { get; set; }
        public string FlightNumber { get; set; }
        public double Price { get; set; }

    }
}
