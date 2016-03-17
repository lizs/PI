
using System.IO;

namespace Pi.Editor
{
    /// <summary>
    ///     编辑器环境配置
    /// </summary>
    public class Environment
    {
        public const string EnvironmentJsonFile = "Environment.json";
        public const string EntityDefPath = "Entity";
        public const string PropertyDefPath = "Block";
        public const string EnumDefPath = "Enum";
        public const string ConstDefPath = "Const";

        private static Environment _ins;
        public static Environment Ins
        {
            get
            {
                if (_ins != null) return _ins;
                _ins = JsonSerializer.Deserialize<Environment>(Path.Combine(FileSys.WorkingDirectory, EnvironmentJsonFile));

                if (!FileSys.DirectoryExists(_ins.TemplatesPath))
                    FileSys.CreateDirectory(_ins.TemplatesPath, false);
                if (!FileSys.DirectoryExists(_ins.ScriptsPath))
                    FileSys.CreateDirectory(_ins.ScriptsPath, false);
                if (!FileSys.DirectoryExists(_ins.DllOutputPath))
                    FileSys.CreateDirectory(_ins.DllOutputPath, false);
                if (!FileSys.DirectoryExists(_ins.DefPath))
                    FileSys.CreateDirectory(_ins.DefPath, false);

                return _ins;
            }
        }
        /// <summary>
        ///     模板根
        /// </summary>
        public string TemplatesPath { get; set; }

        /// <summary>
        ///     脚本根
        /// </summary>
        public string ScriptsPath { get; set; }

        /// <summary>
        ///     dll输出路径
        /// </summary>
        public string DllOutputPath { get; set; }

        /// <summary>
        ///     定义路径
        /// </summary>
        public string DefPath { get; set; }
    }
}
