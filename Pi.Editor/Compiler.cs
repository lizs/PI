using System.IO;
using System.Text;

namespace Pi.Editor
{
    internal static class Compiler
    {
        public static void Compile()
        {
            // scan scripts
            var cses = new StringBuilder();
            foreach (var cs in FileSys.EnumerateFiles(Environment.Ins.ScriptsPath, "*.cs"))
            {
                cses.AppendFormat(" {0}", cs);
            }

            // gen Pi.Gen.dll
            var args = string.Format(
                @"/target:library /out:{0}/Pi.Gen.dll 
/reference:protobuf-net.dll
/reference:socket4net.dll 
/reference:Pi.Framework.dll
{1}",
                Environment.Ins.DllOutputPath, cses);
            CommandlineTool.Excecut("Csc.exe", args);
        }
    }
}
