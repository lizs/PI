using System.IO;
using System.Linq;
using System.Text;

namespace Pi.Editor
{
    public static class ProtoGenerator
    {
        public static void Gen()
        {
            var files = FileSys.EnumerateFiles(Environment.Ins.DefRoot, "*.proto").ToList();
            if(files.Count == 0) return;

            var filesLst = new StringBuilder();
            foreach (var file in files)
            {
                filesLst.AppendFormat(" -i:{0}", file);
            }
            
            var args = string.Format(@"-o:{0} {1} -ns:Pi.Gen", Path.Combine(Environment.Ins.ScriptsPath, "Proto.cs"),
                filesLst);
            CommandlineTool.Excecut("ProtoGen/ProtoGen.exe", args);
        }
    }
}