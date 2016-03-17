
using System.Collections.Generic;
using System.IO;

namespace Pi.Editor
{
    internal class TemplateProvider
    {
        private static TemplateProvider _ins;
        public static TemplateProvider Ins
        {
            get { return _ins ?? (_ins = new TemplateProvider()); }
        }

        private readonly Dictionary<EDefType, string> _templates = new Dictionary<EDefType, string>();

        public string Get(EDefType type)
        {
            if (!_templates.ContainsKey(type))
                _templates.Add(type, FileSys.ReadFile(Path.Combine(Environment.Ins.TemplatesPath, type.ToString() + ".template")));

            return _templates[type];
        }
    }
}
