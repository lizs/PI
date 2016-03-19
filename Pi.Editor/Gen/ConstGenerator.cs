using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pi.Editor
{
    internal class ConstDef
    {
        /// <summary>
        ///     注释
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        ///     常量类型(int/long/string...)
        /// </summary>
        public string ValueType { get; set; }

        /// <summary>
        ///     类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        ///     枚举值
        /// </summary>
        public List<KeyValuePair<string, string>> Values { get; set; }
    }

    internal class ConstGenerator : Generator
    {
        public ConstGenerator(string defPath) : base(defPath)
        {
        }

        public override void Gen()
        {
            var sb = new StringBuilder();
            var template = TemplateProvider.Ins.Get(EDefType.Const);
            var enumDef = JsonSerializer.Deserialize<ConstDef>(DefinitionPath);

            var content = new StringBuilder();
            enumDef.Values.ForEach(x => content.AppendFormat(
                enumDef.ValueType.ToUpper() == "STRING"
                    ? "        public const {0} {1} = \"{2}\";\r\n"
                    : "{3}public const {0} {1} = {2};\r\n", enumDef.ValueType, x.Key, x.Value, Spaces8));
            sb.AppendFormat(template, enumDef.Comment, enumDef.ClassName, content);

            FileSys.WriteToFile(Path.Combine(Environment.Ins.ScriptsPath, enumDef.ClassName + ".cs"), sb.ToString());
        }
    }
}