using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Pi.Editor
{
    /// <summary>
    ///     属性配置
    /// </summary>
    internal class BlockDef
    {
        private string _mode = "Synchronizable";

        /// <summary>
        ///     属性Id
        /// </summary>
        public string PropertyId { get; set; }

        /// <summary>
        ///     模式
        /// </summary>
        public string Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        /// <summary>
        ///     属性类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     元素类型
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        ///     默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        ///     上限
        /// </summary>
        public string Max { get; set; }

        /// <summary>
        ///     下限
        /// </summary>
        public string Min { get; set; }
        
        /// <summary>
        ///     序列化
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    ///     属性生成定义
    /// </summary>
    internal class BlocksDef
    {
        public string Comment { get; set; }
        public List<BlockDef> Blocks { get; set; }
    }


    internal class BlocksGenerator : Generator
    {
        public BlocksGenerator(string defPath)
            : base(defPath)
        {
        }

        public override void Gen()
        {
            var sb = new StringBuilder();
            var template = TemplateProvider.Ins.Get(EDefType.Block);
            var def = JsonSerializer.Deserialize<BlocksDef>(DefinitionPath);
            BlockCache.Ins.Cache(def);
            
            var blocks = new StringBuilder();
            def.Blocks.ForEach(x =>
            {
                var pid = string.Format("EPid.{0}", x.PropertyId);
                var mode = string.Format("EBlockMode.{0}", x.Mode);
                var defaultValue = string.IsNullOrEmpty(x.DefaultValue)
                    ? string.Format("default({0})", x.ItemType)
                    : x.DefaultValue;
                var min = string.IsNullOrEmpty(x.Min)
                    ? string.Format("{0}.MinValue", x.ItemType)
                    : x.DefaultValue;
                var max = string.IsNullOrEmpty(x.Max)
                    ? string.Format("{0}.MaxValue", x.ItemType)
                    : x.DefaultValue;

                var block = string.Empty;
                switch (x.Type.ToUpper())
                {
                    case "SETTABLE":
                        block = string.Format("new SettableBlock<{0}>((short){1}, {2}, {3})", x.ItemType, pid, defaultValue,
                            mode);
                        break;

                    case "INCREASABLE":
                        block = string.Format("new IncreasableBlock<{0}>((short){1}, {2}, {3}, {4}, {5})", x.ItemType, pid, defaultValue,
                            mode, min, max);
                        break;

                    case "LIST":
                        block = string.Format("new ListBlock<{0}>((short){1}, {2}, {3})", x.ItemType, pid, defaultValue,
                            mode);
                        break;

                    default:
                        throw new NotSupportedException();
                }

                blocks.AppendFormat("\t\t\t\t{{{0}, {1}}},\r\n", pid, block);
            });

            sb.AppendFormat(template, def.Comment, blocks);

            FileSys.WriteToFile(Path.Combine(Environment.Ins.ScriptsPath, "Block.cs"), sb.ToString());
        }
    }
}