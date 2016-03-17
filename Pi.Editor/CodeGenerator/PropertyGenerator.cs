using System.Text;

namespace Pi.Editor
{
    /// <summary>
    ///     属性cs文件生成
    /// </summary>
    internal class EnumGenerator : CodeGenerator
    {
        /// <summary>
        ///     属性枚举
        /// </summary>
        public string Enum { get; set; }

        protected override string Gen()
        {
            var sb = new StringBuilder();
           // sb.AppendFormat("public enum {0}\r\n")
            return sb.ToString();
        }
    }
}
