using System.Collections.Generic;

namespace Models.Contracts
{
    public interface IRouteCalculator
    {
        Journey GetRoute(string origin, string destination, IEnumerable<Flight> flights);
    }
}
