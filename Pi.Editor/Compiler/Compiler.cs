using System.Text;

namespace Pi.Editor
{
    internal static class Compiler
    {
        public static void Compile()
        {
            // scan scripts
            var files = new StringBuilder();
            foreach (var cs in FileSys.EnumerateFiles(Environment.Ins.ScriptsPath, "*.cs"))
            {
                files.AppendFormat(" {0}", cs);
            }

            // gen assembly
            var args =
                string.Format(
                    @"/target:library /out:{0}/Pi.Gen.dll /reference:protobuf-net.dll /reference:Pi.Framework.dll {1}",
                    Environment.Ins.DllOutputPath, files);
            CommandlineTool.Excecut("Csc.exe", args);
        }
    }
}
