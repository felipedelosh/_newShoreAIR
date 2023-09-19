
namespace Models.Contracts
{
    public interface IAvailability
    {

        Journey GetJourney(ModelRequesFlights requestF, string Currience_selector); // Return a Fligth A, B

        string getAllFlights(); // Return a List with all Fligths

        string getFlightsV0(); // Load a FLights of newShore API
    }

}
