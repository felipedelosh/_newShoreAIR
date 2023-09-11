using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Contracts
{
    public interface IAvailability
    {

        Journey GetJourney(ModelRequesFlights requestF, string Currience_selector);

        string getFlightsV0();
        string getFlightsV1();
        string getFlightsV2();

        string getGraph();
    }

}
