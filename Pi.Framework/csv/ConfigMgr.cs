using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using socket4net;

namespace Pi.Framework
{
    public abstract class KeyValue : IKeyValue
    {
        private readonly Dictionary<string, string> _dic = new Dictionary<string, string>();

        public void Add(string key, string value)
        {
            _dic.Add(key, value);
        }

        public string Get(string key)
        {
            return _dic.ContainsKey(key) ? _dic[key] : string.Empty;
        }

        public bool Has(string key)
        {
            return _dic.ContainsKey(key);
        }
    }

    public class ConfigMgrArg : ObjArg
    {
        public IFileLoader FileLoader { get; private set; }
        public string NameSpace { get; private set; }
        public string Assembly { get; private set; }
        public bool MultiThreadEnabled { get; private set; }
        public string ConfigRoot { get; private set; }

        public ConfigMgrArg(IObj parent, IFileLoader loader, string configRoot = "Config/", bool mtenabled = false, string ns = null, string assembly = null) : base(parent)
        {
            FileLoader = loader;
            NameSpace = ns;
            MultiThreadEnabled = mtenabled;
            Assembly = assembly;
            ConfigRoot = configRoot;
        }
    }

    public class ConfigMgr : SlicedObj
    {
        // for IBatch<string>
        public ConfigMgr()
        {
            if (Instance != null)
            {
                throw new Exception("ConfigMgr already Instantiated!");
            }
            Instance = this;
        }

        public static ConfigMgr Instance { get; private set; }

        // 存放配置文件相对路径
        readonly List<string> _summary = new List<string>();

        // 存放csv配置
        readonly Dictionary<string, object> _csvConfigs = new Dictionary<string, object>();

        // 存放key-value配置
        readonly Dictionary<string, IKeyValue> _kvConfigs = new Dictionary<string, IKeyValue>();

        // 存放struct配置
        readonly Dictionary<string, object> _structConfigs = new Dictionary<string, object>();

        private IFileLoader _loader;
        private bool _multiThreadEnabled;
        private string _configNamespace = "socket4net";
        private string _configAssemblyName = "socket4net";

        /// <summary>
        ///     Config namespace
        /// </summary>
        public string ConfigNamespace
        {
            get { return _configNamespace; }
        }

        /// <summary>
        ///     Assembly name where config class belongs to
        /// </summary>
        public string ConfigAssemblyName
        {
            get { return _configAssemblyName; }
        }
        
        // 根据Id获取csv配置
        public T GetCsv<T>(int id) where T : class, ICsv
        {
            var cfg = GetCsv<T>();
            return cfg.ContainsKey(id) ? cfg[id] : default(T);
        }

        // 根据类型获取csv配置
        public Dictionary<int, T> GetCsv<T>() where T : ICsv
        {
            var cfg = GetCsv(typeof (T));
            return cfg as Dictionary<int, T>;
        }

        // 根据类型获取csv配置
        public object GetCsv(Type type)
        {
            var key = type.FullName;
            return _csvConfigs.ContainsKey(key) ? _csvConfigs[key] : null;
        }

        /// <summary>
        /// 获取Key-Vaue配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetKeyValue<T>() where T : class, IKeyValue
        {
            var key = typeof (T).FullName;
            return _kvConfigs.ContainsKey(key) ? _kvConfigs[key] as T: null;
        }

        /// <summary>
        ///     获取value
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue<TClass>(string key) where TClass : KeyValue
        {
            var cfg = GetKeyValue<TClass>();
            return cfg != null ? cfg.Get(key) : string.Empty;
        }

        /// <summary>
        ///     获取结构化配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetStruct<T>()
        {
            var key = typeof(T).FullName;
            return (T)(_structConfigs.ContainsKey(key) ? _structConfigs[key] : null);
        }
        
        public TU GetCsv<T, TU>(int i, int j)
            where T : IRichCsv<TU>, new()
            where TU : class, ICsv
        {
            var cfg = GetCsv<T>();
            if (!cfg.ContainsKey(i)) return null;
            var t = cfg[i];

            return t.Children.ContainsKey(j) ? t.Children[j] : null;
        }

        private bool ParseStruct(Type gen, string typeName, string path)
        {
            string fullName;
            string fullNameWioutAssembly;
            var type = GetType(typeName, out fullName, out fullNameWioutAssembly);

            var generic = gen.MakeGenericType(new[] { type });
            var constructor = generic.GetConstructor(new[] { typeof(IFileLoader) });
            var parser = constructor.Invoke(new object[] { _loader }) as IStructParser;
            if (!parser.Parse(path))
                return false;

            _structConfigs[fullNameWioutAssembly] = parser.Config;
            return true;
        }

        private bool ParseKeyValue(string typeName, string path)
        {
            string fullName;
            string fullNameWioutAssembly;
            var type = GetType(typeName, out fullName, out fullNameWioutAssembly);

            var config = Activator.CreateInstance(type) as IKeyValue;
            var parser = new KeyValueParser(_loader);
            if (!parser.Parse(config, path))
                return false;

            _kvConfigs[fullNameWioutAssembly] = config;
            return true;
        }

        bool ParseCsv(Type gen, string typeName, string path)
        {
            string fullName;
            string fullNameWioutAssembly;
            var type = GetType(typeName, out fullName, out fullNameWioutAssembly);

            var generic = gen.MakeGenericType(new[] { type });
            var constructor = generic.GetConstructor(new[] { typeof(IFileLoader) });
            var parser = constructor.Invoke(new object[] { _loader }) as IParser;
            if (!parser.Parse(path))
                return false;

            _csvConfigs[fullNameWioutAssembly] = parser.Config;
            return true;
        }

        private Type GetType(string typeName, out string fullName, out string fullNameWithoutAssembly)
        {
            fullNameWithoutAssembly = string.Concat(ConfigNamespace, ".", typeName);
            fullName = string.Concat(fullNameWithoutAssembly, ",", ConfigAssemblyName);

            var type = Type.GetType(fullName);
            if (type == null)
            {
                Logger.Ins.Fatal("Type of config : [{0}] doesn't exist!!!", fullName);
                throw new Exception(string.Format("Type of config : [{0}] doesn't exist!!!", fullName));
            }
            return type;
        }

        // for task
        protected override void OnInit(ObjArg arg)
        {
            base.OnInit(arg);

            var more = arg.As<ConfigMgrArg>();
            _multiThreadEnabled = more.MultiThreadEnabled;
            _loader = more.FileLoader;
            if (!string.IsNullOrEmpty(more.NameSpace))
                _configNamespace = more.NameSpace;
            if (!string.IsNullOrEmpty(more.Assembly))
                _configAssemblyName = more.Assembly;

            Collect();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Instance = null;
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (_multiThreadEnabled)
            {
                var t = new Thread(LoadAllInWorkerThread);
                t.Start();
            }
            else
            {
                LoadAll();
            }
        }

        private void LoadAllInWorkerThread()
        {
            var ret = true;
            foreach (var path in _summary)
            {
                ret = LoadOne(path);
                if(!ret)
                    break;
            }

            if(ret)
                GlobalVarPool.Ins.LogicService.Perform(() => IsDone = true);
        }

        private void LoadAll()
        {
            var ret = true;
            foreach (var path in _summary)
            {
                ret = LoadOne(path);
                if (!ret)
                    break;
            }

            if (ret)
                IsDone = true;
        }

        private void Collect()
        {
            // collect files
            _summary.AddRange(FileSys.EnumerateFilesRelativeWithoutExt("Config/", "*.*", SearchOption.AllDirectories).ToList());
            Steps = _summary.Count;
        }

        private bool LoadOne(string path)
        {
            Logger.Ins.Info("Parse [{0}] started.", path);
            var info = path.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            var ret = true;
            switch (info[0].ToUpper())
            {
                case "CSV":
                    ret = ParseCsv(typeof(Parser<>), info[1], path);
                    break;

                case "RICHCSV":
                    ret = ParseCsv(typeof(RichParser<>), info[1], path);
                    break;

                case "KEYVALUE":
                    ret = ParseKeyValue(info[1], path);
                    break;

                case "STRUCT":
                    ret = ParseStruct(typeof(StructParser<>), info[1], path);
                    break;

                default:
                    throw new Exception(string.Format("Unknown type : {0}", info[0]));
            }

            if (ret)
            {
                Logger.Ins.Info("Parse [{0}] success!", path);
                ++Progress;
            }

            return ret;
        }
    }
}
