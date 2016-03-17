using System.IO;
using System.Text;

namespace Pi.Editor
{
    /// <summary>
    ///     cs�ļ�������
    /// </summary>
    internal abstract class CodeGenerator
    {
        /// <summary>
        ///     �ļ���׺
        /// </summary>
        private string _extension = "cs";
        public string Extention
        {
            get { return _extension; }
            set { _extension = value; }
        }

        /// <summary>
        ///     �ļ���
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     ģ��
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        ///     ��
        /// </summary>
        public string Root { get; set; }

        /// <summary>
        ///     ȫ��
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