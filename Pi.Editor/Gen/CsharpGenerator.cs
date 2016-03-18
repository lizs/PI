using System;
using System.Linq;

namespace Pi.Editor
{
    internal static class CsharpGenerator
    {
        /// <summary>
        ///     ����cs����
        /// </summary>
        /// <param name="lite">true:�����ɷ�Entity����, false:ȫ������</param>
        public static void Gen(bool lite)
        {
            // scan definition jsons
            var jsons = FileSys.EnumerateFiles(Environment.Ins.DefRoot, "*.json");
            if (lite)
            {
                var entityDir = FileSys.GetWorkingDirectory(Environment.Ins.EntitiesDefPath);
                jsons = jsons.Where(x => FileSys.GetWorkingDirectory(x) != FileSys.GetWorkingDirectory(entityDir));
            }

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