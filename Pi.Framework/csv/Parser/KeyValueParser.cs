using System;
using socket4net;

namespace Pi.Framework
{
    /// <summary>
    ///     key-value parser
    /// </summary>
    public class KeyValueParser : IKeyValueParser
    {
        private readonly IFileLoader _loader;
        public KeyValueParser(IFileLoader loader)
        {
            _loader = loader;
        } 

        public bool Parse(IKeyValue container, string path)
        {
            var text = _loader.Read(path);
            if (string.IsNullOrEmpty(text)) 
            {
                Logger.Ins.Fatal("Parse {0} failed!", path);
                return false;
            }

            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                // # ... => comment
                if (line.TrimStart(' ')[0] == '#')
                    continue;

                var words = line.Split(new [] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length != 2)
                {
                    throw new Exception("Invalid key-value configuration file : " + path);
                }
                
               container.Add(words[0], words[1]);
            }

            return true;
        }
    }
}