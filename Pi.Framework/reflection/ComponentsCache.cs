
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using socket4net;

namespace Pi.Framework
{
    /// <summary>
    ///     获取所有 组件ID=》组件 的映射关系
    /// </summary>
    public static class ComponentsCache
    {
        private static readonly Dictionary<short, Type> _cache = new Dictionary<short, Type>();
        private static readonly Dictionary<Type, short> _inverserCache = new Dictionary<Type, short>();


        public static void Cache(Assembly assembly)
        {
            DoCache(Assembly.GetAssembly(typeof(ComponentsCache)));
            DoCache(assembly);
        }

        private static void DoCache(Assembly assembly)
        {
            try
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsAbstract || !type.IsSubclassOf(typeof(RpcComponent))) continue;

                    var usage =
                        (AttributeUsageAttribute)
                            typeof(ComponentIdAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false)
                                .FirstOrDefault();

                    var inherited = usage == null || usage.Inherited;
                    var attribute = type.GetCustomAttributes(typeof(ComponentIdAttribute), inherited)
                        .Cast<ComponentIdAttribute>().First();

                    var componentId = attribute.Id;

                    if (_cache.ContainsKey(componentId))
                    {
                        var existedType = _cache[componentId];
                        if (type.IsSubclassOf(existedType))
                            _cache[componentId] = existedType;
                    }
                    else
                        _cache[componentId] = type;

                    if (!_inverserCache.ContainsKey(type))
                        _inverserCache[type] = componentId;
                }
            }
            catch (ReflectionTypeLoadException e)
            {
                Logger.Ins.Exception("", e);
            }
        }

        public static Type Get(short componentId)
        {
            return _cache.ContainsKey(componentId) ? _cache[componentId] : null;
        }

        public static short Get(Type type)
        {
            return _inverserCache.ContainsKey(type) ? _inverserCache[type] : short.MaxValue;
        }
    }
}
