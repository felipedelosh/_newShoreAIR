
namespace Models.Contracts
{
    public interface IAvailability
    {

        Journey GetJourney(ModelRequesFlights requestF, string Currience_selector); // Return a Fligth A, B

        string GetAllFlights(); // Return a List with all Fligths

        string GetFlightsV0(); // Load a FLights of newShore API
    }

}
