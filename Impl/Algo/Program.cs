﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Algo
{
    internal static class Program
    {
        private static long GetMicro(Stopwatch s)
        {
            return s.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
        } 
        private static void Show()
        {
            var graph = Generate.GenerateGraph(40, 100, 1); 
            var s = JsonConvert.SerializeObject(graph, Formatting.Indented);

            var dl = Solve.Levit(graph, 1);
            var dd = Solve.Dijkstra(graph, 1);

            var diff = dl.All(entry => dd[entry.Key] == entry.Value);
            
            Console.WriteLine(JsonConvert.SerializeObject(dl, Formatting.Indented));
            Console.WriteLine(JsonConvert.SerializeObject(dd, Formatting.Indented));
            
            File.WriteAllText("../../../show/json/graph1.json", s);
            Console.WriteLine(diff);
        }
        private static void BenchOne(int n, double p)
        {
            var edge = (int) Math.Pow(n * n, p);
            var graph = Generate.GenerateGraph(n, edge, 1337);
            var s = new Stopwatch();
            var results = new Dictionary<string, List<long>> { {"L", new List<long>()}, {"D", new List<long>()} };
            
            Console.WriteLine($"n: {n}, p: {p}, Levit");
            for (var i = 0; i <= 10; ++i)
            {
                s.Restart();   
                Solve.Levit(graph, 1);
                if (i == 0) continue;
                var t = GetMicro(s);
                results["L"].Add(t);  
            }
            
            Console.WriteLine($"n: {n}, p: {p}, Deikstra");
            for (var i = 0; i <= 10; ++i)
            {
                s.Restart();
                Solve.Dijkstra(graph, 1);
                if (i == 0) continue;
                var t = GetMicro(s);
                results["D"].Add(t);
            }
            
            var sResult = JsonConvert.SerializeObject(results, Formatting.Indented);

            File.WriteAllText($"../../../show/json/n-{n}-p-{p}-r.json", sResult);
            if (n >= 1000 && p > 0.5) return;
            var sGraph = JsonConvert.SerializeObject(graph, Formatting.Indented);
            File.WriteAllText($"../../../show/json/n-{n}-p-{p}-g.json", sGraph);
        }

        private static void Bench()
        {
            var ps = new List<double> { 0.3, 0.5, 0.65, 0.7, 0.75, 0.8, 0.9 };
            var ns = new List<int> { 10, 50, 100, 1000, 2000, 3000, 5000, 6000, 10000 };
            foreach (var n in ns)
            {
                foreach (var p in ps)
                {
                    BenchOne(n, p);
                }
            }
        }
        private static void Main()
        {
            Bench();
            // Show();
        }
    }
}