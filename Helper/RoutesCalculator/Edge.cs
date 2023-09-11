using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.RoutesCalculator
{
    /// <summary>
    /// This is a complementary class of Graph.
    /// Storages B, Weight of point.
    /// </summary>
    public class Edge
    {
        public string _nodeB { get; }
        public double _weight { get; }



        public Edge(string nodeB, double weight)
        {
            _nodeB = nodeB;
            _weight = weight;
        }
    }
}
