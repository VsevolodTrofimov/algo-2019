using System;
using System.Collections.Generic;
using System.Linq;

namespace Algo
{
    public static class Solve
    {
        public static Dictionary<int, int> Levit(Generate.Graph g, int s)
        {
            var d = g.ToDictionary(pair => pair.Key, pair => int.MaxValue);
            d[s] = 0;
            var computed = new SortedSet<int>();
            var unknown = new SortedSet<int>();
            var enqueued = new SortedSet<int>();
            var todo = new Queue<int>();
            var todoPriority = new Queue<int>();

            foreach (var v in g.Keys.Where(gValue => gValue != s))
            {
                unknown.Add(v);
                d[v] = int.MaxValue;
            }

            computed.Add(s);
            todo.Enqueue(s);
            enqueued.Add(s);

            while (todo.Count > 0 || todoPriority.Count > 0)
            {
                var u = todoPriority.Count > 0 ? todoPriority.Dequeue() : todo.Dequeue();
                enqueued.Remove(u);
                foreach (var v in g[u].Keys)
                {    
                    if(u == v) continue;
                    var altDistance = d[u] + g[u][v];
                    var shouldRelax = d[v] > altDistance;
                    
                    if (unknown.Contains(v))
                    {
                        unknown.Remove(v);
                        todo.Enqueue(v);
                        d[v] = altDistance;
                        continue;
                    }

                    if (!shouldRelax) continue;
                    
                    d[v] = altDistance;
                    if (!enqueued.Contains(v))
                    {
                        todoPriority.Enqueue(v);
                    }
                }
            }
            
            return d;
        }

        public static Dictionary<int, int> Dijkstra(Generate.Graph g, int s)
        {
            var unvisited = new SortedSet<int>();
            var d = new Dictionary<int, int>();
            foreach (var v in g.Keys)
            {
                unvisited.Add(v);
                d[v] = int.MaxValue;
            }
            d[s] = 0;
            
            while (unvisited.Count > 0)
            {
                var v = d.Keys.Where(key => unvisited.Contains(key)).OrderBy(key => d[key]).First();
                unvisited.Remove(v);
                if (d[v] == int.MaxValue) continue;
                
                foreach (var (u, w) in g[v])
                {
                    if (!unvisited.Contains(u)) continue;
                    
                    var altDistance = d[v] + w;
                    if (d[u] > altDistance)
                    {
                        d[u] = altDistance;
                    }
                }
            }

            return d;
        }
    }
}