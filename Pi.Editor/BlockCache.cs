
using System;
using System.Collections.Generic;

namespace Pi.Editor
{
    internal class BlockCache
    {
        private static BlockCache _ins;
        public static BlockCache Ins
        {
            get { return _ins ?? (_ins = new BlockCache()); }
        }

        private readonly Dictionary<string, string> _cache = new Dictionary<string, string>();

        public void Cache(BlocksDef def)
        {
            _cache.Clear();
            def.Blocks.ForEach(x =>
            {
                var pid = string.Format("EPid.{0}", x.PropertyId);
                var block = string.Empty;
                switch (x.Type.ToUpper())
                {
                    case "SETTABLE":
                        block = string.Format(@"
        public {0} {1}
        {{
            get {{ return Get<{0}>((short){2}); }}
            set {{ Set<{0}>((short){2}, value); }}
        }}",
                            x.ItemType, x.PropertyId, pid);
                        break;

                    case "INCREASABLE":
                        block = string.Format(@"
        public {0} {1}
        {{
            get {{ return Get<{0}>((short){2}); }}
            set {{ IncTo<{0}>((short){2}, value); }}
        }}",
                            x.ItemType, x.PropertyId, pid);
                        break;

                    case "LIST":
                        block = string.Format(@"
        public List<{0}> {1}
        {{
            get {{ return GetList<{0}>((short){2}); }}
            set {{ RemoveAll((short){2}); AddRange<{0}>((short){2}, value); }}
        }}",
                            x.ItemType, x.PropertyId, pid);
                        break;

                    default:
                        throw new NotSupportedException();
                }

                _cache.Add(x.PropertyId, block);
            });
        }

        public string Get(string pid)
        {
            return _cache.ContainsKey(pid) ? _cache[pid] : null;
        }
    }
}
