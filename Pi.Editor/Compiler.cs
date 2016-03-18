using System.IO;
using System.Text;

namespace Pi.Editor
{
    internal static class Compiler
    {
        public static void Compile(bool lite)
        {
            // scan scripts
            var nonEntities = new StringBuilder();
            foreach (var cs in
                    FileSys.EnumerateFiles(Environment.Ins.ScriptsPath, "*.cs", SearchOption.TopDirectoryOnly))
            {
                nonEntities.AppendFormat(" {0}", cs);
            }

            // gen Pi.Gen.dll
            var args = string.Format(
                @"/target:library /out:{0}/Pi.Gen.dll 
/reference:protobuf-net.dll
/reference:socket4net.dll 
/reference:Pi.Framework.dll
{1}",
                Environment.Ins.DllOutputPath, nonEntities);
            CommandlineTool.Excecut("Csc.exe", args);

            if (lite) return;

            // gen Pi.Gen.Entity.dll
            var entities = new StringBuilder();
            foreach (var cs in
                FileSys.EnumerateFiles(Environment.Ins.EntityScriptsPath, "*.cs"))
            {
                entities.AppendFormat(" {0}", cs);
            }

            args = string.Format(
                @"/target:library /out:{0}/Pi.Gen.Entity.dll 
/reference:protobuf-net.dll
/reference:socket4net.dll 
/reference:Pi.Framework.dll
/reference:{1}
/reference:{2}
{3}",
                Environment.Ins.DllOutputPath, Environment.Ins.ComponentsAssembly,
                Path.Combine(Environment.Ins.DllOutputPath, "Pi.Gen.dll"), entities);
            CommandlineTool.Excecut("Csc.exe", args);
        }
    }
}
