using System;
using System.IO;
using Newtonsoft.Json;

namespace Algo
{
    internal static class Program
    {
        private static void Main()
        {
            var graph = Generate.GenerateGraph(3, 4, 15);
            var s = JsonConvert.SerializeObject(graph);

            File.WriteAllText("../../../show/json/graph1.json", s);
            Console.WriteLine("Hello World!");
        }
    }
}