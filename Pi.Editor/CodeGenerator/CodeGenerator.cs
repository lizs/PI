using System.IO;
using System.Text;

namespace Pi.Editor
{
    /// <summary>
    ///     cs文件生成器
    /// </summary>
    internal abstract class CodeGenerator
    {
        /// <summary>
        ///     文件后缀
        /// </summary>
        private string _extension = "cs";
        public string Extention
        {
            get { return _extension; }
            set { _extension = value; }
        }

        /// <summary>
        ///     文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     模板
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        ///     根
        /// </summary>
        public string Root { get; set; }

        /// <summary>
        ///     全名
        /// </summary>
        public string FullName
        {
            get { return Path.Combine(Root, FileName + "." + Extention); }
        }

        public void Generate()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Pi.Framework");
            sb.AppendLine("namespace Pi.Gen");
            sb.AppendLine("{");
            sb.Append(Gen());
            sb.AppendLine("}");
        }

        protected abstract string Gen();
    }
}