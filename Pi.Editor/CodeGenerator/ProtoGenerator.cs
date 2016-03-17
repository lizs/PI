using System.IO;
using System.Text;

namespace Pi.Editor
{
    public static class ProtoGenerator
    {
        public static void Gen()
        {
            var files = new StringBuilder();
            foreach (var file in FileSys.EnumerateFiles(Environment.Ins.DefPath, "*.proto"))
            {
                files.AppendFormat(" -i:{0}", file);
            }

            var args = string.Format(@"-o:{0} {1} -ns:Pi.Gen", Path.Combine(Environment.Ins.ScriptsPath, "Proto.cs"),
                files);
            CommandlineTool.Excecut("tool/ProtoGen.exe", args);
        }
    }
}