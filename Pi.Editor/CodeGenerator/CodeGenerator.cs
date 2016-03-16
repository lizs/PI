using System.IO;

namespace Pi.Editor
{
    /// <summary>
    ///     cs�ļ�������
    /// </summary>
    internal class CodeGenerator
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
    }
}