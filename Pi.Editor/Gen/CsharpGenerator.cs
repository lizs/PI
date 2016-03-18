using System;
using System.Linq;

namespace Pi.Editor
{
    internal static class CsharpGenerator
    {
        public static void Gen()
        {
            // scan definition jsons
            var jsons = FileSys.EnumerateFiles(Environment.Ins.DefRoot, "*.json");

            // generate cs files
            Console.WriteLine("Scan definitions...");
            foreach (var json in jsons)
            {
                var lastNode = FileSys.GetNodes(json).LastOrDefault();
                var upper = string.IsNullOrEmpty(lastNode) ? "" : lastNode;
                GeneratorFactory.Create((EDefType)Enum.Parse(typeof(EDefType), upper, true), json).Gen();
            }
        }
    }
}