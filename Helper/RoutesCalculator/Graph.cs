using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.RoutesCalculator
{
    public class Graph
    {
        public Dictionary<string, List<Edge>> node;

        public Graph()
        {
            node = new Dictionary<string, List<Edge>>();
        }

        public void AddNode(string key)
        {
            if (!node.ContainsKey(key))
            {
                node[key] = new List<Edge>();
            }
        }

        public void addEdge(string nodeA, string nodeB, double weight) {
            if (node.ContainsKey(nodeA) && node.ContainsKey(nodeB))
            {
                node[nodeA].Add(new Edge(nodeB, weight));
            }
        }

            
        public string getRoutes() {

            string route = "";

            foreach (var n in node)
            {
                route = route + $"Node: {n.Key} \n";
                foreach (var a in n.Value)
                {
                    route = route + $"({a._nodeB}, Weight: {a._weight})\n";
                }
                route += "\n";
            }

            return route;
        }


    }
}
