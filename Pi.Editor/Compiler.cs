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

            // compiler
            var systemRoot = System.Environment.GetEnvironmentVariable("SYSTEMROOT");
            var compiler = Path.Combine(systemRoot, "Microsoft.NET/Framework64",
                Environment.Ins.Framework == "net35" ? "v3.5" : "v4.0.30319", "csc.exe");

            // gen Pi.Gen.dll
            var args = string.Format(
                @"/target:library /out:{0}/Pi.Gen.dll 
/reference:lib/{2}/protobuf-net.dll
/reference:lib/{2}/socket4net.dll 
/reference:lib/{2}/Pi.Framework.dll
{1}",
                Environment.Ins.DllOutputPath, cses, Environment.Ins.Framework);
            CommandlineTool.Excecut(compiler, args);
        }
    }
}
