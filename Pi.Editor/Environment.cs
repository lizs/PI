using System.IO;

namespace Pi.Editor
{
    /// <summary>
    ///     编辑器环境配置
    /// </summary>
    public class Environment
    {
        public const string EnvironmentJsonFile = "Environment.json";
        private static Environment _ins;
        private const string _templatesPath = "Templates";
        private const string _scriptsPath = "Scripts";
        private string _dllOutputPath = "Assembly";
        private string _outputFileName = "Pi.Gen";
        private const string _defRoot = "Definitions";
        private readonly string _blocksDefPath = string.Format("Definitions/{0}", EDefType.Block);
        private readonly string _constsDefPath = string.Format("Definitions/{0}", EDefType.Const);
        private readonly string _enumsDefPath = string.Format("Definitions/{0}", EDefType.Enum);
        private readonly string _entitiesDefPath = string.Format("Definitions/{0}", EDefType.Entity);
        private const string ProtosDefDefPath = "Definitions/Proto";

        public static Environment Ins
        {
            get
            {
                if (_ins != null) return _ins;

                var cfgPath = Path.Combine(FileSys.WorkingDirectory, EnvironmentJsonFile);
                _ins = File.Exists(cfgPath) ? JsonSerializer.Deserialize<Environment>(cfgPath) : new Environment();

                CreateDirectories(new[]
                {
                    _ins.TemplatesPath, _ins.ScriptsPath, _ins.DllOutputPath, _ins.BlocksDefPath, _ins.ConstsDefPath,
                    _ins.EnumsDefPath, _ins.EntitiesDefPath, _ins.ProtosDefPath, _ins.EntityScriptsPath
                });

                return _ins;
            }
        }

        private static void CreateDirectories(string[] paths)
        {
            if(paths == null || paths.Length == 0) return;
            foreach (var path in paths)
            {
                if (!FileSys.DirectoryExists(path))
                    FileSys.CreateDirectory(path, false);
            }
        }

        public string EntitiesDefPath
        {
            get { return _entitiesDefPath; }
        }

        public string EnumsDefPath
        {
            get { return _enumsDefPath; }
        }

        public string ConstsDefPath
        {
            get { return _constsDefPath; }
        }

        public string BlocksDefPath
        {
            get { return _blocksDefPath; }
        }

        /// <summary>
        ///     模板根
        /// </summary>
        public string TemplatesPath
        {
            get { return _templatesPath; }
        }

        /// <summary>
        ///     脚本根
        /// </summary>
        public string ScriptsPath
        {
            get { return _scriptsPath; }
        }
        
        /// <summary>
        ///     脚本根
        /// </summary>
        public string EntityScriptsPath
        {
            get { return string.Format("{0}/{1}", ScriptsPath, EDefType.Entity); }
        }

        /// <summary>
        ///     proto根
        /// </summary>
        public string ProtosDefPath
        {
            get { return ProtosDefDefPath; }
        }

        /// <summary>
        ///     dll输出路径
        /// </summary>
        public string DllOutputPath
        {
            get { return _dllOutputPath; }
            set { _dllOutputPath = value; }
        }

        /// <summary>
        ///     定义路径
        /// </summary>
        public string DefRoot
        {
            get { return _defRoot; }
        }

        /// <summary>
        ///     组件程序集
        /// </summary>
        public string ComponentsAssembly { get; set; }

        /// <summary>
        ///     输出文件名
        /// </summary>
        public string OutputFileName
        {
            get { return _outputFileName; }
            set { _outputFileName = value; }
        }
    }
}
