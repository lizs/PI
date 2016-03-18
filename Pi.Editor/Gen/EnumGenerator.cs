using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pi.Editor
{
    internal class EnumDef
    {
        /// <summary>
        ///     注释
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        ///     枚举名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     枚举值
        /// </summary>
        public List<string> Values { get; set; }
    }

    /// <summary>
    ///     枚举文件生成
    /// </summary>
    internal class EnumGenerator : Generator
    {
        public EnumGenerator(string defPath) : base(defPath)
        {
        }

        public override void Gen()
        {
            var sb = new StringBuilder();
            var template = TemplateProvider.Ins.Get(EDefType.Enum);
            var enumDef = JsonSerializer.Deserialize<EnumDef>(DefinitionPath);

            var content = new StringBuilder();
            enumDef.Values.ForEach(x => content.AppendLine(string.Format("\t\t{0},", x)));
            sb.AppendFormat(template, enumDef.Comment, enumDef.Name, content);

            FileSys.WriteToFile(Path.Combine(Environment.Ins.ScriptsPath, enumDef.Name + ".cs"), sb.ToString());
        }
    }
}
