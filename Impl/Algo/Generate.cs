using System;
using System.Collections.Generic;


namespace Algo
{
    public  static class Generate
    {
        private const int MaxWeight = 100;

        public class Graph :Dictionary<int, Dictionary<int, int>> { }
        
        public static Graph GenerateGraph(int nodes, int edges, int seed) 
        {
            var random = new Random(seed);
            var graph = new Graph();
            for (var i = 0; i < nodes; ++i)
            {
                graph.Add(i, new Dictionary<int, int>());
            }

            
            for (var i = 0; i < edges; ++i)
            {
                var n = random.Next(0, nodes);
                var v = random.Next(0, nodes);
                var ov = v;
                var on = n;
                if (v == n)
                {
                    v = (n + 1) % nodes;
                }
                while (graph[n].ContainsKey(v))
                {
                    v = (v + 1) % nodes;
                    n = (n + 1) % nodes;
                    if (v == ov || n == on)
                    {
                        break;
                    }
                }
                graph[n][v] = random.Next(0, MaxWeight);
            }

            return graph;
        }
    }
}