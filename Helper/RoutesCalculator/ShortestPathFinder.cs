using System;
using System.Collections.Generic;
using System.Linq;

namespace Helper.RoutesCalculator
{
    public class ShortestPathFinder
    {
        private Graph graph;
        public bool isValidRoute;

        public ShortestPathFinder(Graph graph)
        {
            this.graph = graph;
            isValidRoute = false;
        }

        /// <summary>
        /// This is my first vr of dijkstra
        /// Note: the alg it´s not complete 
        /// if the graph have unconecte node get infinity loop
        /// </summary>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns></returns>
        public List<String> FindShortestPath(string nodeA, string nodeB) {

            List<string> shortestPath = new List<string>();

            //Node A == B
            if (nodeA == nodeB) {
                shortestPath.Add("The origin and destination are same");
                return shortestPath;
            }

            //Node A,B in Graph
            if (!graph.havKey(nodeA) || !graph.havKey(nodeB)) {
                string msg = "";
                msg += !graph.havKey(nodeA) ? $"The Origin {nodeA} is not FOUND\n" : $"The Origin {nodeA} is ok\n";
                msg += !graph.havKey(nodeB) ? $"The Origin {nodeB} is not FOUND\n" : $"The Origin {nodeB} is ok\n";
                shortestPath.Add(msg);
                isValidRoute = false;
                return shortestPath;
            }

            //Calculate
            var distances = new Dictionary<string, double>();
            var previousNodes = new Dictionary<string, string>();
            var unvisitedNodes = new List<string>(graph.node.Keys);

            foreach (var n in unvisitedNodes)
            {
                distances[n] = double.MaxValue;
            }

            var startNode = nodeA;
            distances[startNode] = 0;

            //Warning infinity loop
            var counter = 0;
            while (unvisitedNodes.Count > 0 || counter <= 10000) {

                var currentNode = GetClosestNode(unvisitedNodes, distances);
               
                unvisitedNodes.Remove(currentNode);

                if (distances[currentNode] == double.MaxValue)
                {
                    break; // No more reachable nodes
                }

                foreach (var neighbor in graph.node[currentNode])
                {
                    var tentativeDistance = distances[currentNode] + neighbor._weight;
                    if (tentativeDistance < distances[neighbor._nodeB])
                    {
                        distances[neighbor._nodeB] = tentativeDistance;
                        previousNodes[neighbor._nodeB] = currentNode;
                    }
                }

                counter += 1;
            }

            //NOT CONECTION?
            if (!previousNodes.ContainsKey(nodeB))
            {
                isValidRoute = false;
                shortestPath.Add($"NOT CONECTION {nodeA} : {nodeB}");
                return shortestPath;
            }
            else {
                isValidRoute = true;
            }

            //EXIST A CONECTION GET THE OUTPUT
            var xPath = new Dictionary<string, double>();
            var node = nodeB;
            while (node != null)
            {
                xPath[node] = distances[node];
                node = previousNodes.ContainsKey(node) ? previousNodes[node] : null;
            }

            //Reconstruct ROUTE
            foreach (var item in xPath)
            {
                shortestPath.Add(item.Key);
            }

            return Enumerable.Reverse(shortestPath).ToList();

        }


        private string GetClosestNode(List<string> unvisitedNodes, Dictionary<string, double> distances)
        {
            return unvisitedNodes.OrderBy(node => distances[node]).First();
        }



    }
}
