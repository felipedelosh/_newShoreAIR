﻿
namespace Models.Contracts
{
    public interface IAvailability
    {

        Journey GetJourney(ModelRequesFlights requestF, string Currience_selector);

        string getFlightsV0();

        string getGraph();
    }

}
