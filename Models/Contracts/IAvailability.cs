﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Contracts
{
    public interface IAvailability
    {
        List<Flight> ListFlights(ModelRequesFlights requestF);
    }

}
