using System.Collections.Generic;

namespace Helper.RoutesCalculator
{
    /// <summary>
    /// This is a main class to Graph
    /// Storages A,B points with the weight
    /// An calculate the shortes path
    /// </summary>
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


        public bool havKey(string key) {
            return node.ContainsKey(key);
        }

        public List<string> getAllKeys() {
            List<string> keys = new List<string>(node.Keys);
            return keys;
        }

        public void addEdge(string nodeA, string nodeB, double weight) {
            if (node.ContainsKey(nodeA) && node.ContainsKey(nodeB))
            {
                node[nodeA].Add(new Edge(nodeB, weight));
            }
        }

        /// <summary>
        /// Retrun a String of Graph
        /// nodeA:\n
        /// (NodeB, w)
        /// (NodeC, w)
        /// ...
        /// </summary>
        /// <returns></returns>
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


        public bool isEmpty() {
            return node.Count != 0;
        }


    }
}
